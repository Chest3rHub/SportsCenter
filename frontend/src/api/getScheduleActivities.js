import API_URL from "../appConfig";

export default async function getScheduleActivities(token,offset){
    return fetch(`${API_URL}/SportActivities/Get-schedule-activities?offset=${offset}`, {

        method: 'GET',
        headers: {
          'Content-Type': 'application/json'
        }
      });
}