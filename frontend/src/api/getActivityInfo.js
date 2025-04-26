import API_URL from "../appConfig";

export default async function getActivityInfo(token,id){
    return fetch(`${API_URL}/SportActivities/get-sport-activity-with-id-${id}`, {

        method: 'GET',
        headers: {
          'Content-Type': 'application/json'
        }
      });
}