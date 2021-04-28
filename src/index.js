import React, { Component } from 'react';
import ReactDOM from 'react-dom';

import Bar from './components/bar/bar'
import SearchPanel from './components/search-panel/search-panel'
import Card from './components/card/card';


import './card-list.css'
import './components/button-colored/button-colored.css'



class App extends Component {

  
   state = {
    cardArr: [
    {
      name: 'Туса джуса',
      location: 'У параши',
      date: 'Когда то там',
      //color: chooseColor
    },
    {
      name: 'Туса хуюса',
      location: 'У параши',
      date: 'Когда то там',
      //color: chooseColor
    },
    {
      name: 'Туса мурса',
      location: 'У параши',
      date: 'Когда то там',
      //color: chooseColor
    },

  ]
}



  render() {

    
      return (
      <div>
        <Bar />
        <div className='firstLine'>
          <SearchPanel />
          <div>
            <button className='buttonColored'> + Создать мероприятие</button>
          </div>
        </div>
        <div className='secondLine'>
          <Card cardArr={this.state.cardArr} />
        </div>

      </div>

    );

  }
}

export default App;