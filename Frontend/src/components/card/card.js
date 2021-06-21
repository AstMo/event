//@ts-check
import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import ProgressBar from '../progress-bar/progress-bar';


import './card.css';



const testData = [
    {
        bgcolor: '#171738',
        completed: 60
    },
];

//let randomInteger = (min, max) => {
//   let rand = min - 0.5 + Math.random() * (max - min + 1);
//return Math.round(rand);
// }

const colorArr = ['#432DD4', '#7180B9', '#A5A3AE', '#BDBBC7', '#857CC6'];


//let color = randomInteger(1, 5);

//let chooseColor = colorArr[color];

//style={{backgroundColor: carditem.chooseColor}}

function Card(props) {
    
            return (
                <div>123</div>
                // <Link to='/eventCards'>
                //     <div className='card'  >
                //         <div className='colorPart' style={{ backgroundColor: '#432DD4' }}>
                //             <div className='firstLine content'>
                //                 <h5 className='location'>{event.address}</h5>
                //                 <h5 className='date'>{event.date}</h5>
                //             </div>
                //             <div className='progressBar content'>
                //                 {testData.map((item, idx) => (
                //                     <ProgressBar key={idx} bgcolor={item.bgcolor} completed={item.completed} />
                //                 ))}

                //             </div>

                //         </div>

                //         <div className='lightPart content'>
                //             <div className='eventName'>
                //                 <h4>{event.name}</h4>
                //             </div>

                //         </div>

                //     </div>
                // </Link>
            );
       


};


export default Card;