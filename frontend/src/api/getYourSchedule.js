import API_URL from "../appConfig";

export default async function getYourSchedule(offset){
    return fetch(`${API_URL}/SportActivities/get-your-activities-by-weeks?weekOffset=${offset}`, {

        method: 'GET',
        headers: {
          'Content-Type': 'application/json'
        },
        credentials: 'include',
      });
}