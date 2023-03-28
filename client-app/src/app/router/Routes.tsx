import { RouteObject } from "react-router";
import { createBrowserRouter } from "react-router-dom";
import ActivityDashboard from "../../features/activities/dashboard/ActivityDashboard";
import ActvitiyDetails from "../../features/activities/details/ActivityDetails";
import ActivityForm from "../../features/activities/form/ActivityForm";
import App from "../layout/App";

export const routes: RouteObject[] = [
    {
        path: '/',
        element: <App />,
        children: [
            {path: 'activities', element: <ActivityDashboard />},
            {path: 'activities/:id', element: <ActvitiyDetails />},
            {path: 'manage/:id', element: <ActivityForm key='manage' />},
            {path: 'createActivity', element: <ActivityForm key='create' />},
        ]
    }
]

export const router = createBrowserRouter(routes)