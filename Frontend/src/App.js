//@ts-check
import React, { Component, useEffect, useState } from 'react';
import ReactDOM from 'react-dom';
import { Route, Switch, Redirect, withRouter, Router, BrowserRouter } from "react-router-dom"
import Bar from './components/bar/bar'
import Card from './components/card/card';
import Entry from './pages/entry/entry/entry';
import Registration from './pages/registration/registration';
import createHistory from 'history/createBrowserHistory';
import ColumnsCards from "./components/columnsinevent/columnsinevent";

import ProgressBar from './components/progress-bar/progress-bar';

import './card-list.css'
import './components/button-colored/button-colored.css'
import HelloPage from './pages/hello-page/hello-page';
import Statistic from './pages/statistics/statistics';
import DoughnutChart from './pages/statistics/chart-statistics';
import ForgotPassword from './pages/forgot-password/forgot-password';
import EventCreate from './pages/event-create/event-create';
import { Link } from "react-router-dom"
import { instanceOf } from 'prop-types';
import { withCookies, Cookies } from 'react-cookie';
import { configConsumerProps } from 'antd/lib/config-provider';

export const history = createHistory();

class App extends Component {
  static propTypes = {
    cookies: instanceOf(Cookies).isRequired
  }
  constructor(props) {


    super(props);


    const { cookies } = props;
    this.state = {
      cardArr: [
        {
          name: 'Отпраздновать окончание ВУЗа',
          location: 'У Максима',
          date: '15 июня 2021 18:00',
          //color: chooseColor
        },
        {
          name: 'Запуск онлайн-школы',
          location: 'Москва Сити',
          date: '20 июля 2021 10:00',
          //color: chooseColor
        },
        {
          name: 'Свадьба Коли',
          location: 'Село Коли',
          date: '20 июля 2021 14:00',
          //color: chooseColor
        },

      ],
      user: cookies.get('user') || ''


    }
  }

  componentDidMount() {
    const { cookies } = this.props;
    this.state.user = cookies.get('user')
  }


  render() {
    console.log(this.state.user);
    return (


      <div className="App">
        <Bar user={this.state.user} />
        <BrowserRouter>
          <Router history={history}>
            <Route exact path='/' component={HelloPage}></Route>
            <Route exact path='/entry' component={Entry}></Route>
            <Route exact path='/forgotpassword' component={ForgotPassword}></Route>
            <Route exact path='/registration' component={Registration}></Route>
            <Route exact path='/create-event' component={EventCreate}></Route>
            <Route exact path='/eventCards' component={ColumnsCards}></Route>
            <Route exact path='/events'><UserEvents props={this.state} /></Route>
          </Router>
        </BrowserRouter>


      </div>

    );

  }
}

function UserEntry(params) {
  return <Entry></Entry>
}

function UserHelloPage(params) {
  return <HelloPage></HelloPage>
}

function UserRegistration(params) {
  return <Registration></Registration>
}

function UserForgotPassword(params) {
  return <ForgotPassword></ForgotPassword>
}


function UserStatistic(params) {
  return <DoughnutChart></DoughnutChart>
}

function UserEventCreate(params) {
  return <EventCreate></EventCreate>
}

const testData = [
  {
    bgcolor: '#171738',
    completed: 60
  },
];


function UserEvents(props) {

  const [events, setEvents] = useState([])

  useEffect(() => {
    (async () => {
      let response = await fetch(`http://cv-dentistry.ru/api/Event/items/1/15?filter_isdeleted=false`, {
        method: 'GET',
        headers: {
          'Authorization': `Bearer ${props.props.user.token}`
        },

      })
      if (response.ok) {
        let res = await response.json()
        setEvents(res.result.items)
      }
    })()
  }, [])
console.log(events);
  return (
    <>
      <div className='firstLine'>


        <Link to={{ pathname: `/create-event`, state: { token: props.props.user.token, id: props.props.user.id } }} >
          <div>

            <button className='buttonColored'> + Создать мероприятие</button>

          </div>
        </Link>
      </div>
      <div className='secondLine'>
        {events && events.map((event) => {
          return(
          <Link to={{pathname: '/eventCards', state: {id: event.id, token: props.props.user.token, userid: props.props.user.id}}}>
            <div className='card'  >
              <div className='colorPart' style={{ backgroundColor: '#432DD4' }}>
                <div className='firstLine content'>
                  <h5 className='location'>{event.address}</h5>
                  <h5 className='date'>{event.date}</h5>
                </div>
                <div className='progressBar content'>
                  {testData.map((item, idx) => (
                    <ProgressBar key={idx} bgcolor={item.bgcolor} completed={item.completed} />
                  ))}

                </div>

              </div>

              <div className='lightPart content'>
                <div className='eventName'>
                  <h4>{event.name}</h4>
                </div>

              </div>

            </div>
          </Link>
          )
        })}

      </div>
    </>
  )

}

export default withCookies(App)