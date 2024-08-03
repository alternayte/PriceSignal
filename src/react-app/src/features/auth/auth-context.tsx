import { createContext, useContext } from 'react';
import { User } from 'firebase/auth';

export const AuthContext = createContext<AuthContextType | null>(null);
export const useAuth = () => useContext(AuthContext);

export type AuthUser = User;

export interface AuthContextType {
    user: AuthUser | null;
    userLoading: boolean;
    signup: (email: string, password: string) => Promise<User>;
    signin: (email: string, password: string) => Promise<User>;
    signinWithProvider: (name: string) => Promise<User>;
    signinWithRedirect: (name: string) => Promise<void>;
    signout: () => Promise<void> | null;
    redirectResult: () => Promise<User | null>;
    // sendPasswordResetEmail: (email: string) => Promise<void>
    // confirmPasswordReset: (password: string, code: string) => Promise<void>
    // updatePassword: (password: string) => Promise<void>
    // updateProfile: (data: any) => Promise<void>
}