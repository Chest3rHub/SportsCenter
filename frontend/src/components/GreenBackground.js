import '../styles/greenBackground.css';

export default function GreenBackground({children, height, marginTop}){
    return(
        <div className='green-background' style={{minHeight: height, marginTop: marginTop,}}>
            <div className='centered-content'>
              {children}
            </div>
        </div>
    );
}