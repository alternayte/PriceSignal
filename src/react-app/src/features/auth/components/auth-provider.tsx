// Initialize Firebase auth
import { firebaseApp } from '@/lib/firebase';
import {
    createUserWithEmailAndPassword,
    FacebookAuthProvider,
    getAdditionalUserInfo,
    getAuth,
    GithubAuthProvider,
    GoogleAuthProvider,
    onAuthStateChanged,
    onIdTokenChanged,
    sendEmailVerification,
    signInWithEmailAndPassword,
    signInWithPopup,
    signInWithRedirect,
    signOut as authSignOut,
    TwitterAuthProvider,
    User,
    UserCredential,
    getRedirectResult,
} from 'firebase/auth';
import React, { useEffect, useState } from 'react';
import { AuthContext } from '../auth-context';
// import { useToast } from '../ui/use-toast';
const EMAIL_VERIFICATION = false;

const auth = getAuth(firebaseApp);

// setPersistence(auth, inMemoryPersistence);

// This should wrap the app in `src/pages/_app.js`
interface AuthContextProps {
    children: React.ReactNode;
}

export const AuthProvider = ({ children }: AuthContextProps) => {
    const auth = useAuthProvider();
    return <AuthContext.Provider value={auth}>{children}</AuthContext.Provider>;
};

function useAuthProvider() {
    const [user, setUser] = useState<User | null>(null);
    const [userLoading, setUserLoading] = useState<boolean>(true);
    const [, setToken] = useState<string | null>(null);

    // Merge extra user data from the database
    // This means extra user data (such as payment plan) is available as part
    // of `auth.user` and doesn't need to be fetched separately. Convenient!
    // let finalUser = useMergeExtraData(user, { enabled: MERGE_DB_USER })

    // Add custom fields and formatting to the `user` object
    // finalUser = useFormatUser(finalUser)

    const handleSignInRedirect = async (response: UserCredential) => {
        const { user } = response;
        if (!user) {
            return null;
        }

        const jwt = await user.getIdToken();

        await fetch('/api/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${jwt}`,
            },
        });

        return handleAuth(response);
    };

    // Handle response from auth functions (`signup`, `signin`, and `signinWithProvider`)
    const handleAuth = async (response: UserCredential) => {
        const { user } = response;
        const additional = getAdditionalUserInfo(response);

        // Ensure Firebase user is ready before we continue
        await waitForFirebase(user.uid);

        // store token in memory
        const jwt = await user.getIdToken();
        setToken(jwt);

        // Create the user in the database if they are new.
        if (additional?.isNewUser) {
            if (EMAIL_VERIFICATION) {
                await sendEmailVerification(auth.currentUser ? auth.currentUser : user);
            }
        }

        // Update user in state
        setUser(user);
        return user;
    };

    const signup = (email: string, password: string) => {
        return createUserWithEmailAndPassword(auth, email, password).then(handleAuth);
    };

    const signin = (email: string, password: string) => {
        return signInWithEmailAndPassword(auth, email, password).then(handleAuth);
    };

    const signinWithProvider = (name: string) => {
        // Get provider by name ("google", "facebook", etc)
        // @ts-ignore
        const provider = authProviders.find((p) => p.name === name).get();
        return signInWithPopup(auth, provider).then(handleAuth);
    };

    const signinWithRedirect = (name: string) => {
        // @ts-ignore
        const provider = authProviders.find((p) => p.name === name).get();
        return signInWithRedirect(auth, provider);
    };

    const redirectResult = () => {
        return getRedirectResult(auth).then((result) => {
            if (result) {
                return handleSignInRedirect(result);
            } else {
                return null;
            }
        });
    };

    const signout = () => {
        // DefaultConfig.config = new Configuration({
        //     accessToken: undefined,
        // });
        setToken(null);
        setUser(null);

        authSignOut(auth).then(() => {
            // fetch('/api/logout', {
            //     cache: 'no-store',
            //     method: 'POST',
            //     headers: {
            //         'Content-Type': 'application/json',
            //     },
            // }).then((res) => {
            //     if (res.status === 200) {
            //         navigate(0)
            //         navigate('/')
            //        
            //     }
            // });
        });
        return null;

        // fetch('/api/sign-out', {
        //   method: 'POST',
        //   headers: {
        //     'Content-Type': 'application/json',
        //   },
        // }).then((res) => {
        //   if (res.status === 200) {
        //     // OpenAPI.TOKEN = undefined;
        //     // setToken(null);
        //     // setUser(null);
        //   }
        // });

        //return authSignOut(auth);
    };

    // const sendPasswordResetEmail = (email) => {
    //   return authSendPasswordResetEmail(auth, email)
    // }
    //
    // const confirmPasswordReset = (password, code) => {
    //   // Get code from query string object
    //   const resetCode = code || getFromQueryString('oobCode')
    //   return authConfirmPasswordReset(auth, resetCode, password)
    // }
    //
    // const updatePassword = (password) => {
    //   return authUpdatePassword(auth.currentUser, password)
    // }
    //
    // // Update auth user and persist data to database
    // // Call this function instead of multiple auth/db update functions
    // const updateProfile = async (data) => {
    //   const { email, name, picture } = data
    //
    //   // Update auth email
    //   if (email) {
    //     await authUpdateEmail(auth.currentUser, email)
    //   }
    //
    //   // Update built-in auth profile fields
    //   // These fields are renamed in `useFormatUser`, so when updating we
    //   // need to make sure to use their original names (`displayName`, `photoURL`, etc)
    //   if (name || picture) {
    //     let fields = {}
    //     if (name) fields.displayName = name
    //     if (picture) fields.photoURL = picture
    //     await authUpdateProfile(auth.currentUser, fields)
    //   }
    //
    //   // Persist all data to the database
    //   await updateUser(user.uid, data)
    //
    //   // Update user in state
    //   setUser(auth.currentUser)
    // }

    // const { toast } = useToast();
    // const queryclient = useQueryClient();

    useEffect(() => {
        // Subscribe to user on mount
        const unsubscribe = onAuthStateChanged(auth, (user) => {
            if (user) {
                setUser(user);
                setUserLoading(false);
                user.getIdToken().then((token) => {
                    // DefaultConfig.config = new Configuration({
                    //     accessToken: token,
                    // });
                    setToken(token);
                });

             
            } else {
                setUser(null);
                setUserLoading(false);
            }
        });

        const unsubscribeIdToken = onIdTokenChanged(auth, (user) => {
            if (user) {
                user.getIdToken().then((token) => {
                    // DefaultConfig.config = new Configuration({
                    //     accessToken: token,
                    // });
                    setToken(token);
                    fetch('/api/login', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json',
                            Authorization: `Bearer ${token}`,
                        },
                    });
                    // }).then((res) => {
                    //     //router.push('/');
                    //     // navigate(0)
                    // });
                });
            }
        });

        // Unsubscribe on cleanup
        return () => {
            unsubscribe();
            unsubscribeIdToken();
        };
    }, []);

    return {
        user,
        userLoading,
        signup,
        signin,
        signinWithProvider,
        signout,
        signinWithRedirect,
        redirectResult,
        // sendPasswordResetEmail,
        // confirmPasswordReset,
        // updatePassword,
        // updateProfile,
    };
}

// Wait for Firebase user to be initialized before resolving promise
// and taking any further action (such as writing to the database)
const waitForFirebase = (uid: string) => {
    return new Promise((resolve) => {
        const unsubscribe = onAuthStateChanged(auth, (user) => {
            // Ensure we have a user with expected `uid`
            if (user && user.uid === uid) {
                resolve(user); // Resolve promise
                unsubscribe(); // Prevent from firing again
            }
        });
    });
};

const authProviders = [
    {
        id: 'password',
        name: 'password',
    },
    {
        id: 'google.com',
        name: 'google',
        get: () => new GoogleAuthProvider(),
    },
    {
        id: 'facebook.com',
        name: 'facebook',
        get: () => {
            const provider = new FacebookAuthProvider();
            provider.setCustomParameters({ display: 'popup' });
            return provider;
        },
    },
    {
        id: 'twitter.com',
        name: 'twitter',
        get: () => new TwitterAuthProvider(),
    },
    {
        id: 'github.com',
        name: 'github',
        get: () => new GithubAuthProvider(),
    },
];
