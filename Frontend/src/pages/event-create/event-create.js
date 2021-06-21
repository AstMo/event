import React, { Component } from 'react';
import { Form, Button, Container } from 'react-bootstrap';
import ReactDOM from 'react-dom';
import { Link } from "react-router-dom"
import Input from '../../components/input/Input';
import DropdowSelection from '../../components/list-person/list-person';

import pkg from 'semantic-ui-react/package.json';
import { Dropdown } from 'semantic-ui-react';
import { history } from '../../App';
import "./event-create.css"

class EventCreate extends Component {
    constructor(props) {
        super(props)
        console.log(props)
        this.state = {
            formControls: {
                name: {
                    value: '',
                    type: 'name',
                    label: 'Название мероприятия',
                    errorMassage: 'Введите корректное название',
                    valid: false,
                    touched: false,
                    validation: {
                        required: true,
                        minLength: 3
                    }
                },

                date: {
                    value: '',
                    type: 'date',
                    label: 'Дата мероприятия',
                    errorMassage: 'Введите корректный email',
                    valid: false,
                    touched: false,
                    validation: {
                        required: true,
                        minLength: 8
                    }
                },

                place: {
                    value: '',
                    type: 'name',
                    label: 'Место проведения',
                    errorMassage: 'Введите корректный email',
                    valid: false,
                    touched: false,
                    validation: {
                        required: true,
                        minLength: 2
                    }
                },

                budget: {
                    value: '',
                    type: 'name',
                    label: 'Бюджет',
                    errorMassage: 'Введите корректный email',
                    valid: false,
                    touched: false,
                    validation: {
                        required: true,
                        minLength: 1
                    }
                },

            },
            token: this.props.location.state.token
        }
    }

    enterHandler = () => {
        (async () => {

            let response = await fetch('http://cv-dentistry.ru/api/Event/create', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${this.state.token}`
                },
                body: JSON.stringify({

                    name: this.state.formControls.name.value,
                    date: "2021-10-27T19:34:46.004Z",
                    address: this.state.formControls.place.value,
                    latitude: 15,
                    longitude: 20,
                    typeEvent: 1,
                    totalBudget: parseFloat(this.state.formControls.budget.value),
                    participaties: [
                        {
                            UserId: this.props.location.state.id,
                            role: 1
                        }
                    ]

                })

            })
            if (response.ok) {
                let res = await response.json()
                console.log(res);
                history.push('/events')
                
            }
        })()
    }

    submitHandler = event => {
        event.preventDefault()
    }
    validateControl(value, validation) {
        if (!validation) {
            return true
        }
        let isValid = true
        if (validation.required) {
            isValid = value.trim() !== '' && isValid
        }

        if (validation.minLength) {
            isValid = value.length >= validation.minLength && isValid
        }
        return isValid
    }
    onChangeHandler = (event, controlName) => {
        console.log('${controlName}: ', event.target.value)

        const formControls = { ... this.state.formControls }
        const control = { ...formControls[controlName] }

        control.value = event.target.value
        control.touched = true
        control.valid = this.validateControl(control.value, control.validation)

        formControls[controlName] = control
        this.setState({
            formControls
        })
    }

    renderInputs() {
        return Object.keys(this.state.formControls).map((controlName, index) => {
            const control = this.state.formControls[controlName]
            return (
                <Input
                    key={controlName + index}
                    type={control.type}
                    value={control.value}
                    valid={control.valid}
                    touched={control.touched}
                    label={control.label}
                    shouldValidate={true}
                    errorMassage={control.errorMassage}
                    onChange={event => this.onChangeHandler(event, controlName)}
                />
            )
        })
    }
    render() {
        return (

            <Container style={{ width: '1400px' }} className="createEvent" >

                <Form onSubmit={this.submitHandler} className="">
                    {this.renderInputs()}
                    <div >
                        <textarea class="form-control" className="description" rows="6" placeholder='Введите описание мероприятия'></textarea>

                        <div className="btns">
                            <Link to="/forgotpassword">
                                <button className="btnPurple" onClick={this.enterHandler} >
                                    Назад
                            </button>
                            </Link>
                            <button className="btnLightPurple" onClick={this.enterHandler} >
                                Удалить
                            </button>

                            <button className="buttonColored" onClick={this.enterHandler} >
                                Сохранить
                            </button>
                        </div>
                    </div>

                </Form>

                <div className='orgAndMembers'>

                    <h2 className='orgAndMembers'>Участники и организаторы</h2>
                    <h4 className='orgAndMembers'>Организаторы </h4>
                    <div className='peopleList'>

                        <div className='dropdown'>
                            <DropdowSelection token={this.state.token}/>

                        </div>



                    </div>



                    <h4 className='orgAndMembers'>Участники </h4>

                    <div className='peopleList'>



                        <div className='dropdown'>
                            <DropdowSelection token={this.state.token}/>

                        </div>




                    </div>

                </div>



            </Container >

        )
    }
}



export default EventCreate
