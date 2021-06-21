import React, { Component } from 'react';
import { Form, Button, Container } from 'react-bootstrap';
import { Route, Switch, Redirect, withRouter, Link } from "react-router-dom";
import Input from '../../components/input/Input';
import './registration.css'
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

class Registration extends Component {

    state = {
        formControls: {
            email: {
                value: '',
                type: 'email',
                label: 'Email',
                errorMassage: 'Введите корректный email',
                valid: false,
                touched: false,
                validation: {
                    required: true,
                    email: true
                }
            },
            login: {
                value: '',
                type: '',
                label: 'Логин',
                errorMassage: 'Логин должен быть длиннее 6 символов',
                valid: false,
                touched: false,
                validation: {
                    required: true,
                    minLength: 6
                }
            },
            password: {
                value: '',
                type: 'password',
                label: 'Пароль',
                errorMassage: 'Пароль должен быть длиннее 6 символов',
                valid: false,
                touched: false,
                validation: {
                    required: true,
                    minLength: 6
                }
            },
            passwordRepeat: {
                value: '',
                type: 'password',
                label: 'Повторите пароль',
                errorMassage: 'Пароли не совпадают',
                valid: false,
                touched: false,
                validation: {
                    required: true,
                    onePass: true
                }
            }
        }
    }

    enterHandler = () => {
        (async () => {

            let response = await fetch('http://cv-dentistry.ru/api/Account/registration', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    email: this.state.formControls.email.value,
                    name: this.state.formControls.login.value,
                    password: this.state.formControls.password.value,
                    passwordRepeat: this.state.formControls.passwordRepeat.value,
                    phone: "+79999999999"
                })

            })
            if (response.ok) {
                let res = await response.json()
                console.log(res);
                history.push('/entry')
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
                <img src='logo.png' className="logoEntry" />
                <Form onSubmit={this.submitHandler} className="regForm">
                    {this.renderInputs()}
                    <Form.Group controlId="formBasicCheckbox">
                        <Form.Check type="checkbox" label="Даю согласие на обработку данных" />
                    </Form.Group>
                    <div className='buttonsReg'>

                        <button className="btnReg buttonColored " onClick={this.enterHandler} >
                            Зарегистрироваться
                        </button>


                    </div>

                </Form>
            </Container >
        )
    }
}

export default Registration