// @ts-check

import React, { Component } from 'react';
import { Form, Button, Container } from 'react-bootstrap';
import { Route, Switch, Redirect, withRouter, Link } from "react-router-dom";
import Input from '../../../components/input/Input';
import './entry.css'
import { useHistory } from "react-router-dom";
import PropTypes from 'prop-types';
import { history } from '../../../App';
import { instanceOf } from 'prop-types';
import { withCookies, Cookies } from 'react-cookie';

function validateEmail(email) {
    const re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(email).toLowerCase());
}

class Entry extends Component {

    static propTypes = {
        cookies: instanceOf(Cookies).isRequired
    };

    constructor(props) {

        super(props)
        console.log(props);
        const { cookies } = props;
        this.state = {
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
                password: {
                    value: '',
                    type: 'password',
                    label: 'Пароль',
                    errorMassage: 'Введите корректный пароль',
                    valid: false,
                    touched: false,
                    validation: {
                        required: true,
                        minLength: 6
                    }
                }
            },


        }

    }
    static contextTypes = {
        router: PropTypes.object,
    }

    setCookies(user) {
        const { cookies } = this.props;
        cookies.set('user', user, { path: '/' });
    }

    enterHandler = () => {
        (async () => {

            let response = await fetch('http://cv-dentistry.ru/api/Account/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    email: this.state.formControls.email.value,
                    password: this.state.formControls.password.value,
                })

            })
            if (response.ok) {
                let user = await response.json()
                console.log(user);
                this.setCookies(user.result)
                history.push("/events")
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

            <Container style={{ width: '465px' }} className="entry" >
                <img src='logo.png' className="logoEntry" />
                <Form onSubmit={this.submitHandler} className="entryForm">
                    {this.renderInputs()}

                    <div className='forgotPassword'>
                        <Link className='logo' to="/forgotpassword">
                            <h5>Забыли пароль?</h5>
                        </Link>
                    </div>
                    <div >

                        <button className=" btnEntry1 buttonColored" onClick={this.enterHandler} >
                            Войти
                        </button>


                    </div>

                </Form>
            </Container >
        )
    }
}

export default withCookies(Entry)