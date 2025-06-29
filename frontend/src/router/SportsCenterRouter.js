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
import ActivityDetails from "../pages/ActivityDetails";
import Reservations from "../pages/Reservations";
import AddReservationForClient from "../pages/AddReservationForClient";
import GetReservationInfo from "../pages/GetReservationInfo";
import ReservationsSummary from "../pages/ReservationsSummary";
import MyTimetable from "../pages/MyTimetable";
import RegisterEmployee from "../pages/RegisterEmployee";
import OwnerDashboard from "../pages/OwnerDashboard";
import ClubWorkingHoursPage from "../pages/ClubWorkingHoursPage";
import MyActivityDetails from "../pages/MyActivityDetails";
import NewClientReservation from "../pages/NewClientReservation";
import ForgotPassword from "../pages/ForgotPassword";
import CoachDashboard from "../pages/CoachDashboard";
import CleanerDashboard from "../pages/CleanerDashboard";

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
                element: <NewClientReservation />,
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
                path: '/my-timetable',
                element: <MyTimetable />,
            },   
            {
                path: "/get-sport-activity-with-id",
                element: <GetActivityInfo />
            },
            {
                path: '/activity-details',
                element: <ActivityDetails />,
            },    
            {
                path: '/my-activity-details',
                element: <MyActivityDetails />,
            },                   
            {
                path: "*",
                element: <NotFound />,
            }
        ]
    }
];
    
    const ownerRouter = [
        {
            path: "/",
            element: <OwnerLayout />,
            children: [
                {
                    path: "/",
                    element: <OwnerDashboard />,
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
                    path: '/activity-details',
                    element: <ActivityDetails />,
                },              
                {
                    path: '/reservations',
                    element: <Reservations />,
                },
                {
                    path: '/create-single-reservation-for-client',
                    element: <AddReservationForClient />
                },
                {
                    path: '/get-reservation-with-id',
                    element: <GetReservationInfo />
                },
                {
                    path: "/reservation-summary",
                    element: <ReservationsSummary />
                },
                {
                    path: "/register-employee",
                    element: <RegisterEmployee />
                },
                 {
                     path: "/working-hours",
                     element: <ClubWorkingHoursPage />
                 },
                {
                    path: "*",
                    element: <NotFound />,
                },
            ]
        }
    ];

    const coachRouter = [
        {
            path: "/",
            element: <CoachLayout />,
            children: [
                {
                    path: "/",
                    element: <CoachDashboard />,
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
                    path: '/my-timetable',
                    element: <MyTimetable />,
                },   
                {
                    path: '/activity-details',
                    element: <ActivityDetails />,
                },  
                {
                    path: '/my-activity-details',
                    element: <MyActivityDetails />,
                }, 
                {
                    path: "/get-sport-activity-with-id",
                    element: <GetActivityInfo />
                },
                {
                    path: "*",
                    element: <NotFound />,
                }
            ]
        }
    ];

    const cleanerRouter = [
        {
            path: "/",
            element: <CleanerLayout />,
            children: [
                {
                    path: "/",
                    element: <CleanerDashboard />,
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
                    path: '/activity-details',
                    element: <ActivityDetails />,
                },  
                {
                    path: "/get-sport-activity-with-id",
                    element: <GetActivityInfo />
                },
                {
                    path: "*",
                    element: <NotFound />,
                }
            ]
        }
    ];

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
                    path: "/update-client-deposit",
                    element: <ChangeClientDeposit />
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
                    path: "/trainings",
                    element: <SportActivities />
                },
                 {
                    path: "/Add-activity",
                    element: <AddSportActivity />
                },
                {
                    path: '/activity-details',
                    element: <ActivityDetails />,
                },  
                {
                    path: "/get-sport-activity-with-id",
                    element: <GetActivityInfo />
                },
                {
                    path: '/reservations',
                    element: <Reservations />,
                },
                {
                    path: '/create-single-reservation-for-client',
                    element: <AddReservationForClient />
                },
                {
                    path: '/get-reservation-with-id',
                    element: <GetReservationInfo />
                },
                {
                    path: "/reservation-summary",
                    element: <ReservationsSummary />
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
                    path: '/activity-details',
                    element: <ActivityDetails />,
                },  
                {
                    path: "/get-sport-activity-with-id",
                    element: <GetActivityInfo />
                },
                {
                    path: "/forgot-password",
                    element: <ForgotPassword />
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
        } else if(role ==='Anonim'){
            router = baseRouter;
        } else {
            router = baseRouter;
        }
        return createBrowserRouter(router);
    }

export default function SportsCenterRouter(props){

    const { role,  router, setRouter } = useContext(SportsContext);

    useEffect(() => {
        if (role) {
            const newRouter = getRouter(role); 
            setRouter(newRouter); 
        } else {
            const newRouter = getRouter('Anonim');
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