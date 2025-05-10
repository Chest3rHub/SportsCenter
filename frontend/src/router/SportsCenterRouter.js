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
import News from "../pages/News";
import AddNews from "../pages/AddNews";
import Logout from "../pages/Logout";
import EditNews from "../pages/EditNews";
import NewsDetails from "../pages/NewsDetails";
import ChangePassword from "../pages/ChangePassword";
import Employees from "../pages/Emploees";
import MyAccount from "../pages/MyAccount";
import ClientDashboard from "../pages/ClientDashboard"
import EmployeeDashboard from "../pages/EmployeeDashboard";
import Clients from "../pages/Clients"
import AddClientDiscount from "../pages/AddClientDiscount"
import AddDepositToClient from "../pages/AddDepositToClient";
import ChangeClientDeposit from "../pages/ChangeClientDeposit";
import AddReservationYourself from "../pages/AddReservationYourself";
import ClientReservations from "../pages/ClientReservations";
import MoveReservation from "../pages/MoveReservation";
import ToDoPage from "../pages/ToDo";
import SportActivities from "../pages/SportActivities";
import GetActivityInfo from "../pages/GetActivityInfo";
import AddSportActivity from "../pages/AddSportActivity";
import ActivitiesSummary from "../pages/ActivitiesSummary";
import Timetable from "../pages/Timetable";

// pozmieniac dla klienta na komponenty trasy itd

const clientRouter = [
    {
        path: "/",
        element: <ClientLayout />,
        children: [
            {
                path: "/",
                element: <ClientDashboard />,
            },
            {
                path: "/dashboard",  
                element: <Dashboard />,
            },
            {
                path: "/news",
                element: <News />,
            },
            {
                path: "news/:id", 
                element: <NewsDetails />, 
            },
            {
                path: "/error",  
                element: <Error />,
            },
            {
                path: "/logout",  
                element: <Logout />,
            },
            {
                path: "/change-password",
                element: <ChangePassword />,
            },
            {
                path: '/account',
                element: <MyAccount />,
            },
            {
                path: '/Create-single-reservation-yourself',
                element: <AddReservationYourself />,
            },
            {
                path: '/move-reservation',
                element: <MoveReservation />,
            },
            {
                path: '/my-reservations',
                element: <ClientReservations />,
            }, 
            {
                path: '/timetable',
                element: <Timetable />,
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
                    path: "/news",
                    element: <News />,
                },
                {
                    path: "news/:id", 
                    element: <NewsDetails />, 
                },
                {
                    path: "/add-news",
                    element: <AddNews />,
                },
                {
                    path: "/edit-news",
                    element: <EditNews />,
                },
                {
                    path: "/error",  
                    element: <Error />,
                },
                {
                    path: "/logout",  
                    element: <Logout />,
                },
                {
                    path: "/change-password",
                    element: <ChangePassword />,
                },
                {
                    path: '/account',
                    element: <MyAccount />,
                },
                {
                    path: "/employees",
                    element: <Employees />,
                },
                {
                    path: "/clients",
                    element: <Clients />,
                },
                {
                    path: "/add-client-discount",
                    element: <AddClientDiscount />
                },  
                {
                    path: "/add-deposit-to-client",
                    element: <AddDepositToClient />
                },  
                {
                    path: "/update-client-deposit",
                    element: <ChangeClientDeposit />
                },
                {
                    path: "/todo",
                    element: <ToDoPage />
                },
                {
                    path: "/trainings",
                    element: <SportActivities />
                },
                {
                    path: "/get-sport-activity-with-id",
                    element: <GetActivityInfo />
                },
                {
                    path: "/Add-activity",
                    element: <AddSportActivity />
                },
                {
                    path: "/get-activity-summary",
                    element: <ActivitiesSummary />
                },
                {
                    path: '/timetable',
                    element: <Timetable />,
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
                    path: "/news",
                    element: <News />,
                },
                {
                    path: "news/:id", 
                    element: <NewsDetails />, 
                },
                {
                    path: "/error",  
                    element: <Error />,
                },
                {
                    path: "/logout",  
                    element: <Logout />,
                },
                {
                    path: "/change-password",
                    element: <ChangePassword />,
                },
                {
                    path: '/account',
                    element: <MyAccount />,
                },
                {
                    path: '/timetable',
                    element: <Timetable />,
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
                    path: "/news",
                    element: <News />,
                },
                {
                    path: "news/:id", 
                    element: <NewsDetails />, 
                },
                {
                    path: "/error",  
                    element: <Error />,
                },
                {
                    path: "/logout",  
                    element: <Logout />,
                },
                {
                    path: "/change-password",
                    element: <ChangePassword />,
                },
                {
                    path: '/account',
                    element: <MyAccount />,
                },
                {
                    path: '/timetable',
                    element: <Timetable />,
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
                    element: <EmployeeDashboard />,
                },
                {
                    path: "/dashboard",  
                    element: <Dashboard />,
                },
                {
                    path: "/news",
                    element: <News />,
                },
                {
                    path: "news/:id", 
                    element: <NewsDetails />, 
                },
                {
                    path: "/add-news",
                    element: <AddNews />,
                },
                {
                    path: "/edit-news",
                    element: <EditNews />,
                },
                {
                    path: "/error",  
                    element: <Error />,
                },
                {
                    path: "/logout",  
                    element: <Logout />,
                },
                {
                    path: "/change-password",
                    element: <ChangePassword />,
                },
                {
                    path: '/account',
                    element: <MyAccount />,
                },
                {
                    path: "/clients",
                    element: <Clients />,
                },
                {
                    path: "/add-client-discount",
                    element: <AddClientDiscount />
                },  
                {
                    path: "/add-deposit-to-client",
                    element: <AddDepositToClient />
                },       
                {
                    path: "/todo",
                    element: <ToDoPage />
                },     
                {
                    path: '/timetable',
                    element: <Timetable />,
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
                    path: "/news",
                    element: <News />,
                },
                {
                    path: "news/:id", 
                    element: <NewsDetails />, 
                },
                {
                    path: "/error",  
                    element: <Error />,
                },
                {
                    path: "/logout",  
                    element: <Logout />,
                },
                {
                    path: '/timetable',
                    element: <Timetable />,
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
    }, [role]); 

    if (!router) {
        return null; 
    }
    return <>
    
    <RouterProvider router={router}/>
    </>
    
}