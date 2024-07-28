import '../styles/greenBackground.css';

export default function GreenBackground({children}){
    return(
        <div className='green-background'>
            <div className='centered-content'>
              {children}
            </div>
        </div>
    );
}