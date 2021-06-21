import React, { Component } from 'react';
import { Form, Button, Container } from 'react-bootstrap';
import { Route, Switch, Redirect, withRouter, Link } from "react-router-dom";
import { Icon, Image, Modal } from 'semantic-ui-react'
import Input from '../../components/input/Input';
import './form.css'
import { Dropdown } from 'semantic-ui-react'
import { history } from '../../App';


function validateEmail(email) {
    const re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(email).toLowerCase());
}

function validatePasswords(password, passwordRepeat) {
    if (password == passwordRepeat) {
        return true
    }
    else return false
}

const options = [
    { key: 1, text: 'Высокая', value: 1 },
    { key: 2, text: 'Средняя', value: 2 },
    { key: 3, text: 'Низкая', value: 3 },
]

const DropdownExampleClearable = () => (
    <Dropdown clearable options={options} selection style={{ backgroundColor: '#EDEBF5', width: '50px', borderRadius: '10px', borderColor: '#bebebe', marginBottom: '10px' }} />
)

class FormCustom extends Component {

    constructor(props) {
        super(props)
        console.log(props);
        this.state = {
            formControls: {
                name: {
                    value: '',
                    type: 'name',
                    label: 'Название',
                    errorMassage: 'Введите корректное название',
                    valid: false,
                    touched: false,
                    validation: {
                        required: true,
                        minLength: 2
                    }
                },
                dedline: {
                    value: '',
                    type: 'date',
                    label: 'Дедлайн задачи',
                    errorMassage: 'Введите корректный дедлайн',
                    valid: false,
                    touched: false,
                    validation: {
                        required: true,
                        minLength: 6
                    }
                },
                description: {
                    value: '',
                    type: '',
                    label: 'Описание',
                    errorMassage: 'Пароли не совпадают',
                    valid: false,
                    touched: false,
                    validation: {
                        required: true,
                        minLength: 2
                    }
                },
                budget: {
                    value: '',
                    type: '',
                    label: 'Бюджет',
                    errorMassage: 'Пароли не совпадают',
                    valid: false,
                    touched: false,
                    validation: {
                        required: true,
                        minLength: 2
                    }
                }
            }
        }
    }


    enterHandler = () => {
        (async () => {

            let response = await fetch('http://cv-dentistry.ru/api/Task/create', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${this.props.props.token}`
                },
                body: JSON.stringify({

                    eventId: this.props.props.id,
                    assignedId: this.props.props.userid,
                    name: this.state.formControls.name.value,
                    description: this.state.formControls.description.value,
                    state: parseInt(this.props.props.state),
                    expenseItems: []

                })

            })
            if (response.ok) {
                let res = await response.json()
                console.log(res);               
                history.go(0)
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
        if (validation.email) {
            isValid = validateEmail(value) && isValid
        }
        if (validation.minLength) {
            isValid = value.length >= validation.minLength && isValid
        }
        if (validation.onePass) {
            isValid = validatePasswords(value, this.state.formControls.password.value) && isValid
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

            <Container style={{ width: '500px' }} className="reg" >

                <Form onSubmit={this.submitHandler} className="regForm">
                    {this.renderInputs()}
                    <Button style={{ backgroundColor: '#432DD4' }} onClick={this.enterHandler} primary>
                        Создать <Icon name='chevron right' />
                    </Button>

                </Form>
            </Container >
        )
    }
}

export default FormCustom