import actionTypes from './actionTypes';

export function ShowModalAction(title, body) {
    return {
        type: actionTypes.SHOWMODAL,
        show: true,
        title,
        body
    };
};

export function CloseModalAction() {
    return {
        type: actionTypes.CLOSEMODAL,
        show: false
    };
};