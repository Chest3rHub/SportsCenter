import API_URL from "../appConfig";

export default async function getActivitySummary(token,startDate, endDate) {
    return fetch(`${API_URL}/SportActivities/get-activity-summary?startDate=${startDate}&endDate=${endDate}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    });
}
