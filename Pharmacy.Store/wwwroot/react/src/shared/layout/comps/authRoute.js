import React from 'react';
import { Route, Redirect, withRouter } from 'react-router-dom';
import { connect } from 'react-redux';

class AuthRoute extends React.Component {
  render() {
    let Children = this.props.component;
    return (
      <Route
        exact
        path={this.props.path}
        {...this.props.rest}
        render={({ location }) =>
          this.props.authenticated ? (
            <Children {...this.props} />
          ) : (
              <Redirect
                to={{
                  pathname: "/",
                  state: { from: location }
                }}
              />
            )
        }
      />
    );
  }

}

const mapStateToProps = (state, ownProps) => {
  return { ...ownProps, ...state.authReducer };
}

// const mapDispatchToProps = dispatch => ({
//     logOut: () => dispatch(LogOutAction())
// });

export default connect(mapStateToProps, null)(AuthRoute);