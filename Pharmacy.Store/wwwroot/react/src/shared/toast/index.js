import React from 'react';
import { connect } from 'react-redux';
import { Toast } from 'react-bootstrap';
import { CloseToastAction } from '../../redux/actions/toastAction';

class CustomToast extends React.Component {

  _onClose() {
    this.props.closeToast();
    if (this.props.onClose) this.props.onClose();
  }

  render() {
    //console.log()
    const { title, body, show } = this.props;
    return (
      <Toast show={show}
        onClose={this._onClose.bind(this)}
        delay={3000}
        autohide
        style={{
          position: 'absolute',
          top: 0,
          right: 0,
        }}>
        <Toast.Header>
          <strong>{title}</strong>
        </Toast.Header>
        <Toast.Body>{body}</Toast.Body>
      </Toast>
    );
  }
}

const mapStateToProps = (state, ownProps) => {
  return { ...ownProps, ...state.toastReducer };
}

const mapDispatchToProps = dispatch => ({
  closeToast: () => { dispatch(CloseToastAction()); }
});

export default connect(mapStateToProps, mapDispatchToProps)(CustomToast);
