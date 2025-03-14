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
import EmployeeLayout from "../layouts/EmployeeLayout";
import { SportsContext } from "../context/SportsContext";
import { useContext, useEffect } from "react";
import CleanerLayout from "../layouts/CleanerLayout";
import CoachLayout from "../layouts/CoachLayout";
import ClientLayout from "../layouts/ClientLayout";

// pozmieniac dla klienta na komponenty trasy itd

const clientRouter = [
    {
        path: "/",
        element: <ClientLayout />,
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
    
// pozmieniac dla wlasciciela na komponenty trasy itd
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

    // pozmieniac dla trenera na komponenty trasy itd
    const coachRouter = [
        {
            path: "/",
            element: <CoachLayout />,
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

    // pozmieniac dla sprzataczki na komponenty trasy itd
    const cleanerRouter = [
        {
            path: "/",
            element: <CleanerLayout />,
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

    // pozmieniac dla pracownika adm. na komponenty trasy itd
    const employeeRouter = [
        {
            path: "/",
            element: <EmployeeLayout />,
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
    

    // pozmieniac dla niezalogowanego usera na komponenty trasy itd
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
        if(role==="Klient"){
            router = clientRouter;
        } else if (role === "Wlasciciel"){
            router = ownerRouter;
        } else if (role === "Trener"){
            router = coachRouter;
        } else if (role === "Pomoc sprzatajaca"){
            router = cleanerRouter;
        } else if (role === "Pracownik administracyjny"){
            router = employeeRouter;
        } else {
            router = baseRouter;
        }
        return createBrowserRouter(router);
    }

export default function SportsCenterRouter(props){
    // tutaj mozna bedzie dodac rozpoznawanie slownika z jezykiem polskim i angielskim
    // a rola bedzie wyciagana z props (pewnie backend ja bedzie wysylal)
    const { role,  router, setRouter } = useContext(SportsContext);

    useEffect(() => {
        if (role) {
            const newRouter = getRouter(role); 
            setRouter(newRouter); 
        } else {
            const newRouter = getRouter('anonymous');
            setRouter(newRouter);
        }
    }, [role, setRouter]); 

    if (!router) {
        return null; 
    }
    return <>
    
    <RouterProvider router={router}/>
    </>
    
}