import '../styles/greenBackground.css';

export default function GreenBackground({children}){
    return(
        <div className='green-background'>
            {children}
        </div>
    );
}