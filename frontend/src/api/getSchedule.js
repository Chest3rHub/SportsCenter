import API_URL from "../appConfig";

export default async function getSchedule(offset){
    return fetch(`${API_URL}/Schedule/Schedule-info?weekOffset=${offset}`, {

        method: 'GET',
        headers: {
          'Content-Type': 'application/json'
        },
        credentials: 'include',
      });
}