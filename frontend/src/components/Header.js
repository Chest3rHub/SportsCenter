import '../styles/header.css';

export default function Header({children}){
    return(
        <div className="auth-header">
             {children}
        </div>
    );
}
