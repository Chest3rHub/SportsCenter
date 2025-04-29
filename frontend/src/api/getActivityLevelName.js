import API_URL from "../appConfig";

export default async function getActivityLevelNames() {
    const response = await fetch(`${API_URL}/SportActivities/get-activities-level-names`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        },
        credentials: 'include',
    });

    const data = await response.json();
    return data;
}