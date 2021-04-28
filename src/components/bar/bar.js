import React from 'react';

import './bar.css';


const Bar = () => {
    return (
        <div className = 'bar'>
            <img src = 'logo.png' className = 'logo'/>
            <button type="button"
              className="btn btn-sm btnHome">
            <i className="fas fa-home"></i>
            </button>

            <button type="button"
              className="btn btn-sm btnPerson">
                <i className="fas fa-user"></i>
              </button>
        </div>
      );
    };

export default Bar;