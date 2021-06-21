import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import { Link } from "react-router-dom"

import './hello-page.css'


class HelloPage extends Component {

  render() {

    return (
      <div className="helloText fontFamily">
        <div className="helloText1">

          <h1 style={{ fontWeight: 600 }}>Приветствую!</h1>
          <h2 style={{ fontWeight: 300 }}>Мы рады видеть Вас на странице нашего приложения MyEvent,</h2>
          <h2 style={{ fontWeight: 300 }}>которое поможет Вам в организации мероприятия</h2>

          <div className="helloText2">
            <h3 style={{ fontWeight: 600 }}>Вы можете войти в существующий аккаунт или зарегистрироваться:</h3>
          </div>



          <div className='buttons'>
            <Link to="/entry">
              <button className='buttonColored btnEnter'> Войти </button>
            </Link>

            <Link to="/registration">
              <button className='buttonColored'> Зарегистрироваться </button>
            </Link>

          </div>
        </div>
      </div>



    );

  }
}

export default HelloPage
