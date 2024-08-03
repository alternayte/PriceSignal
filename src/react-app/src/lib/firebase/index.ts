import { getApp, getApps, initializeApp } from 'firebase/app';

// Initialize Firebase
let app;

if (getApps().length === 0) {
    // Initialize Firebase app

    app = initializeApp({
        apiKey: import.meta.env.VITE_FIREBASE_API_KEY as string,
        authDomain: import.meta.env.VITE_FIREBASE_AUTH_DOMAIN as string,
        projectId: import.meta.env.VITE_FIREBASE_PROJECT_ID as string, 
        storageBucket: import.meta.env.VITE_FIREBASE_STORAGE_BUCKET as string,
        messagingSenderId: import.meta.env.VITE_FIREBASE_MESSAGING_SENDER_ID as string,
        appId: import.meta.env.VITE_FIREBASE_APP_ID as string, 
        measurementId: import.meta.env.VITE_FIREBASE_MEASUREMENT_ID as string,
    });
} else {
    // Use existing app if already initialized
    app = getApp();
}

export const firebaseApp = app;
