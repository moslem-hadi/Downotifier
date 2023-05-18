import { useEffect, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { yupResolver } from '@hookform/resolvers/yup';
import { useFieldArray, useForm } from 'react-hook-form';
import * as Yup from 'yup';
import {
  singleJobApiCallActions,
  jobApiCallUpsertActions,
  loadingActions,
} from '../_store';

function UpsertForm({ id, afterSubmit }) {
  const dispatch = useDispatch();
  const { jobApiCall } = useSelector(x => x.jobApiCall);
  const [error, setError] = useState(undefined);

  useEffect(() => {
    if (id) getJobApiCall(parseInt(id));

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  useEffect(() => {
    if (id) {
      let defaultValues = {
        id: jobApiCall.id,
        title: jobApiCall?.title,
        url: jobApiCall.url,
        method: jobApiCall.method,
        jsonBody: jobApiCall.jsonBody,
        headers: jobApiCall.headers,
        monitoringInterval: jobApiCall.monitoringInterval,
        notifications: jobApiCall.notifications
      };
      if (defaultValues.headers)
        defaultValues.headers = Object.keys(jobApiCall.headers).map(function (k) { return k + ':' + jobApiCall.headers[k] }).join("&");
      reset({ ...defaultValues });
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [jobApiCall]);
  const getJobApiCall = async id => {
    dispatch(loadingActions.startLoading());
    await dispatch(singleJobApiCallActions.GetById({ id }));
    dispatch(loadingActions.stopLoading());
  };

  const validationSchema = Yup.object().shape({
    title: Yup.string().required('Title is required'),
    url: Yup.string().required('Url is required'),
    monitoringInterval: Yup.number()
      .integer()
      .typeError('interval must be a number')
      .required('interval is required.')
      .positive(),

  });
  const formOptions = {
    resolver: yupResolver(validationSchema),
  };

  const { control, register, handleSubmit, formState, reset } = useForm(formOptions);


  const { fields, append, prepend, remove, swap, move, insert } = useFieldArray({
    control, // control props comes from useForm (optional: if you are using FormContext)
    name: "notifications", // unique name for your Field Array
  });
  const { errors, isSubmitting } = formState;

  async function onSubmit(model) {
    try {
      await dispatch(
        jobApiCallUpsertActions.createJobApiCall(model)
      ).unwrap();
      afterSubmit();
    } catch (ex) {
      setError(ex.message);
    }
  }
  const appendNotification = () => {
    append({ receiver: '', message: '', type: 0 });
  }

  return (
    <>
      {id && <h1 className="text-center">{jobApiCall.name}</h1>}
      <form onSubmit={handleSubmit(onSubmit)}>
        {id && <input type="hidden" {...register('id')} name="id" />}

        <div className="modal-body">
          {error && (
            <div className="alert alert-danger text-center">{error}</div>
          )}
          <div className="row">
            <div className="col-6">
              <div className="mb-3">
                <label className="form-labels">Title</label>
                <input
                  name="title"
                  type="text"
                  {...register('title')}
                  className={`form-control  mt-1 ${errors.title ? 'is-invalid' : ''
                    }`}
                />
                <div className="invalid-feedback">{errors.title?.message}</div>
              </div>
            </div>
            <div className="col-6">
              <div className="mb-3">
                <label className="form-labels">Url</label>
                <input
                  name="url"
                  type="text"
                  {...register('url')}
                  className={`form-control  mt-1 ${errors.url ? 'is-invalid' : ''
                    }`}
                />
                <div className="invalid-feedback">{errors.url?.message}</div>
              </div></div>

            <div className="col-6">
              <div className="mb-3">
                <label className="form-labels">Monitoring Interval<small className='text-muted ms-2'>(Minutes)</small></label>
                <input
                  name="monitoringInterval"
                  type="text"
                  {...register('monitoringInterval')}
                  className={`form-control  mt-1 ${errors.monitoringInterval ? 'is-invalid' : ''
                    }`}
                />
                <div className="invalid-feedback">{errors.monitoringInterval?.message}</div>
              </div>
            </div>
            <div className="col-6">
              <div className="mb-3">
                <label className="form-labels">Method</label>
                <div className='mt-2'>
                  <label>
                    <input
                      name="method"
                      defaultChecked
                      type="radio"
                      defaultValue={0}
                      {...register('method', {
                        valueAsNumber: true,
                      })}
                    />
                    <span className='ms-1'>Get</span>
                  </label>
                  <label className='ms-4'>
                    <input
                      name="method"
                      type="radio"
                      defaultValue={1}
                      {...register('method', {
                        valueAsNumber: true,
                      })}

                    />
                    <span className='ms-1'>Post</span>
                  </label>
                  <div className="invalid-feedback">{errors.method?.message}</div>
                </div>
              </div>
            </div>

            <div className="col-6">
              <div className="mb-3">
                <label className="form-labels">Json Body<small className='text-muted ms-2'>(JSON format)</small></label>

                <textarea
                  placeholder="{'item': '1'}"
                  {...register('jsonBody')}
                  className={`form-control  mt-1 ${errors.jsonBody ? 'is-invalid' : ''
                    }`} id="jsonBody" name='jsonBody' rows="3"></textarea>

                <div className="invalid-feedback">{errors.jsonBody?.message}</div>
              </div>
            </div>
            <div className="col-6">
              <div className="mb-3">
                <label className="form-labels">Headers<small className='text-muted ms-2'>(KeyValue format)</small></label>

                <textarea
                  placeholder="item:1&authentication:bearer 123..."
                  {...register('headers')}
                  className={`form-control  mt-1 ${errors.headers ? 'is-invalid' : ''
                    }`} id="headers" name='headers' rows="3"></textarea>

                <div className="invalid-feedback">{errors.headers?.message}</div>
              </div>
            </div>
            <div className="col-12">
              <label className="form-labels mb-2">Notification <small className='text-muted ms-2'>(On failure)</small></label>
              <button type='button'
                className='btn badge bg-secondary ms-3'
                onClick={appendNotification} >+</button>
              <div>
                {fields.map((field, index) => (
                  <div className='mb-2 row' key={field.id}>
                    <div className="col-2">
                      <select name="type" defaultValue={0} id="type"

                        {...register(`notifications.${index}.type`, {
                          valueAsNumber: true,
                        })}
                        className='form-select'>
                        <option value={0}>Email</option>
                      </select>
                    </div>
                    <div className="col-4">
                      <input
                        className='form-control '
                        placeholder='Receiver'
                        {...register(`notifications.${index}.receiver`)}
                      />
                    </div>
                    <div className="col-6">

                      <input
                        className='form-control '
                        placeholder='Message'
                        {...register(`notifications.${index}.message`)}
                      />
                    </div>


                  </div>


                ))}

              </div>

            </div>





          </div>

        </div>
        <div className="modal-footer d-flex align-items-center justify-content-between">
          <button
            disabled={isSubmitting}
            type="submit"
            className="btn btn-primary"
          >
            {isSubmitting && (
              <span className="spinner-border spinner-border-sm me-2"></span>
            )}
            Submit
          </button>
        </div>
      </form>
    </>
  );
}

export { UpsertForm };
