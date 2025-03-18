export default function parseStringToDate(dateString){
    // Z bo czas sie przesuwal o jeden dzien przez jakas strefe czasowa lokalna
    const date = new Date(dateString + 'Z');
    return date.toISOString().split('T')[0];
  };