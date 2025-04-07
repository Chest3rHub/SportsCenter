import API_URL from "../appConfig";

const getYourUpcomingActivities = (token) => {
    return fetch(`${API_URL}/SportActivities/get-your-upcoming-activities`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        },
    });
};

export default getYourUpcomingActivities;