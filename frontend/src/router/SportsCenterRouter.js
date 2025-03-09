import Navbar from "../components/Navbar";
import Home from "../pages/Home";
import Login from "../pages/Login";
import NotFound from "../pages/NotFound";
import Register from "../pages/Register";
import Dashboard from "../pages/Dashboard";
import Error from "../pages/Error";
import BaseLayout from "../layouts/BaseLayout";
import { RouterProvider, createBrowserRouter } from 'react-router-dom';
import OwnerLayout from "../layouts/OwnerLayout";

const clientRouter = [];
    // tutaj podobne routery bedzie mozna zrobic dla pracownika,
    // wlasciciela, klienta itd
    const ownerRouter = [
        {
            path: "/",
            element: <OwnerLayout />,
            children: [
                {
                    path: "/",
                    element: <Home />,
                },
                {
                    path: "/dashboard",  
                    element: <Dashboard />,
                },
                {
                    path: "/error",  
                    element: <Error />,
                },
                {
                    path: "*",
                    element: <NotFound />,
                }
            ]
        }
    ];
    

    const baseRouter = [
        {
            path: "/",
            element: <BaseLayout />,
            children: [
                {
                    path: "/",
                    element: <Home />,
                },
                {
                    path: "/login",
                    element: <Login />,
                },
                {
                    path: "/register",
                    element: <Register />,
                },
                {
                    path: "/dashboard",  
                    element: <Dashboard />,
                },
                {
                    path: "/error",  
                    element: <Error />,
                },
                {
                    path: "*",
                    element: <NotFound />,
                }
            ]
        }
    ];
    
    function getRouter(role){
        let router;
        if(role==="client"){
            router = clientRouter;
        } else if (role === "owner"){
            router = ownerRouter;
        } else {
            router = baseRouter;
        }
        return createBrowserRouter(router);
    }

export default function SportsCenterRouter(props){
    // tutaj mozna bedzie dodac rozpoznawanie slownika z jezykiem polskim i angielskim
    // a rola bedzie wyciagana z props (pewnie backend ja bedzie wysylal)
    return <>
    
    <RouterProvider router={getRouter('rola wyciagana z props')}/>
    </>
    
}