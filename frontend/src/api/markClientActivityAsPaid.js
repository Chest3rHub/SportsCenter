import API_URL from "../appConfig";

export default function markClientActivityAsPaid(email, activityId) {
  return fetch(`${API_URL}/SportActivities/pay-for-client-activity`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({instanceOfActivityId:activityId, ClientEmail: email}),
    credentials: 'include'
  });
};