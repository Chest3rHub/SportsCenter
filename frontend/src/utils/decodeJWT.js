export default function decodeJWT(token) {
    const parts = token.split('.');
    if (parts.length !== 3) {
      //  console.error('Token JWT jest nieprawidłowy.');
        return;
    }
    const header = JSON.parse(atob(parts[0]));
    const payload = JSON.parse(atob(parts[1]));
    return payload;
}