import '../styles/orangeBackground.css';

export default function OrangeBackground({children}){
    return(
        <div className='orange-background'>
            {children}
        </div>
    );
}