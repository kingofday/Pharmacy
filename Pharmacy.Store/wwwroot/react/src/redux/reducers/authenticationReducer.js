import actionTypes from './../actions/actionTypes';
import strings from './../../shared/constant';

const getInitilState = () => {
    if (!localStorage) {
        alert(strings.browserIsOld);
        return {
            token: null,
            userId: null,
            username: ''
        };
    }


    let rep = getUserInfo();
    if (rep.success)
        return {
            token: rep.result.token,
            userId: rep.result.userId,
            username: rep.result.username
        };
    else
        return {
            token: null,
            userId: null,
            username: ''
        };

}
const authenticationReducer = (state = getInitilState(), action) => {
    switch (action.type) {
        case actionTypes.LOGIN:
            return {
                ...state,
                token: action.token,
                userId: action.userId,
                username: action.username
            };
        case actionTypes.LOGOUT:
            return {
                ...state,
                token: null,
                userId: null,
                username: ''
            };
        default:
            return { ...state };
    }
};


export default authenticationReducer;