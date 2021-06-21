import React, { Component } from 'react';
    import { Form, Button, Container } from 'react-bootstrap';
import { Route, Switch, Redirect, withRouter, Link } from "react-router-dom";
import Input from '../../components/input/Input';
import './forgot-password.css'



function validateEmail(email) {
    const re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(email).toLowerCase());
}


class ForgotPassword extends Component {

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
            }
        }
    }

    enterHandler = () => {

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

            <Container style={{ width: '500px' }} className="forgotpass" >
                <img src='logo.png' className="logoForgotPass" />
                <Form onSubmit={this.submitHandler} className="forgotPassForm">
                    {this.renderInputs()}
                    <div >
                        <Link to="/forgotpassword">
                            <button className= " btnForgotPass buttonColored " onClick={this.enterHandler} >
                                Изменить пароль
                            </button>
                        </Link>
                    </div>

                </Form>
            </Container >
        )
    }
}

export default ForgotPassword