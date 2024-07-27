import '../styles/greenButton.css';

export default function GreenButton({type, children, onClick}){
    return(
        <button type={type} className="green-button" onClick={onClick}>
             {children}
        </button>
    );
}