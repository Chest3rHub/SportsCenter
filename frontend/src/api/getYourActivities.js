import API_URL from "../appConfig";

const getYourActivities = (token) => {
    return fetch(`${API_URL}/SportActivities/get-your-activities`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        },
    });
};

export default getYourActivities;