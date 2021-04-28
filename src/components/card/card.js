//@ts-check
import React, { Component } from 'react';
import ProgressBar from '../progress-bar/progress-bar';

import './card.css';



const testData = [
    {
        bgcolor: '#432DD4',
        completed: 60
    },
];

//let randomInteger = (min, max) => {
 //   let rand = min - 0.5 + Math.random() * (max - min + 1);
//return Math.round(rand);
 // }

//const colorArr = ['#432DD4', '#7180B9', '#A5A3AE', '#BDBBC7', '#857CC6'];

//let color = randomInteger(1, 5);

//let chooseColor = colorArr[color];

//style={{backgroundColor: carditem.chooseColor}}

const Card = (props) => {
    const cards = props.cardArr.map((carditem) => {
        return (
            <div className='card'  >
                <div className='colorPart' >
                    <div className='firstLine content'>
                        <h5 className='location'>{carditem.location}</h5>
                        <h5 className='date'>{carditem.date}</h5>
                    </div>
                    <div className='progressBar content'>
                        {testData.map((item, idx) => (
                            <ProgressBar key={idx} bgcolor={item.bgcolor} completed={item.completed} />
                        ))}

                    </div>

                </div>

                <div className='lightPart content'>
                    <div className='eventName'>
                        <h4>{carditem.name}</h4>
                    </div>

                </div>

            </div>
        );
    })




    return (
        cards
    )
};


export default Card;