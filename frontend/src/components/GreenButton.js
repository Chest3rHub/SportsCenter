import '../styles/greenButton.css';

export default function GreenButton({type, children, onClick, style}){
    return(
        <button type={type} className="green-button" onClick={onClick} style={style}>
             {children}
        </button>
    );
}