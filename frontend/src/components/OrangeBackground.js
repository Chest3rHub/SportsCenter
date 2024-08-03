import '../styles/orangeBackground.css';

export default function OrangeBackground({children, width}){
    return(
        <div className='orange-background' style={{width: width}}>
            {children}
        </div>
    );
}