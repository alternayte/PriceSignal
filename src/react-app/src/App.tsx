import { useMemo } from 'react';
import './App.css';
import { createRouter } from './routes';
import {RouterProvider} from "react-router-dom";

function App() {
  const router = useMemo(() => createRouter(),[]);
  return (
    <RouterProvider router={router}/>
  );
}

export default App;
