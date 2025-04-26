import API_URL from "../appConfig";

export default async function deleteActivity(id, token) {
    return fetch(`${API_URL}/SportsActivity/remove-sport-activity/?sportActivityId=${id}`, {

        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json'
        },
        credentials: 'include',
    });
}