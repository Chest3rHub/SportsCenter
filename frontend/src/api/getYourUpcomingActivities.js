import API_URL from "../appConfig";

const getYourUpcomingActivities = (token) => {
    return fetch(`${API_URL}/SportActivities/get-your-upcoming-activities`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        },
        credentials: 'include',
    });
};

export default getYourUpcomingActivities;