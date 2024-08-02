import Login from "../pages/Login";
import Register from "../pages/Register";
import { RouterProvider, createBrowserRouter } from 'react-router-dom';

const clientRouter = [];
    // tutaj podobne routery bedzie mozna zrobic dla pracownika,
    // wlasciciela, klienta itd
    const adminRouter = [];
    const baseRouter = [
        {
            path: "/home",
            element: <Login/>,
        },
        {
            path: "/login",
            element: <Login/>
        },
        {
            path: "register",
            element: <Register/>
        },
        {
            path: "*",
            element: <Register/>
        }
    ];
    function getRouter(role){
        let router;
        if(role==="client"){
            router = clientRouter;
        } else if (role === "admin"){
            router = adminRouter;
        } else {
            router = baseRouter;
        }
        return createBrowserRouter(router);
    }

export default function SportsCenterRouter(){
    // tutaj mozna bedzie dodac rozpoznawanie slownika z jezykiem polskim i angielskim
    // a rola bedzie wyciagana z props (pewnie backend ja bedzie wysylal)
    return <RouterProvider router={getRouter('rola wyciagana z props')}/>
    
}