import React from 'react';
import { Stepper, Step, StepLabel } from '@material-ui/core';
import strings from './../../shared/constant';

const steps = [strings.selectAddress,strings.selectDeliveryType,strings.review]
export default class Steps extends React.Component {
    render() {
        return (
            <div className='steps-comp'>
                <Stepper activeStep={this.props.activeStep} alternativeLabel>
                    {steps.map((label) => (
                        <Step key={label}>
                            <StepLabel>{label}</StepLabel>
                        </Step>
                    ))}
                </Stepper>
            </div>
        );
    }
}
