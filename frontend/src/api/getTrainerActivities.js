import API_URL from "../appConfig";

export default async function getTrainerActivities(offset){
    return fetch(`${API_URL}/SportActivities/get-trainer-activities-by-weeks?weekOffset=${offset}`, {

        method: 'GET',
        headers: {
          'Content-Type': 'application/json'
        },
        credentials: 'include',
      });
}