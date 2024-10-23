import { observer } from 'mobx-react-lite';
import { useCallback } from 'react';
import { useDropzone } from 'react-dropzone';
import { Header, Icon } from 'semantic-ui-react';

interface Props {
  setFiles: (files: object[]) => void;
}

export default observer(function PhotoWidgetDropzone({setFiles}: Props) {
  const dzStyles = {
    border: 'dashed 3pz #eee',
    borderColor: '#eee',
    borderRadius: '5px',
    paddingTop: '30px',
    textAlign: 'center',
    height: 200
  } as object

  const dzActive = {
    borderColor: 'green'
  }

  //useCallback is a optimazation feature
  const onDrop = useCallback((acceptedFiles: object[]) => {
    setFiles(acceptedFiles.map((file: object) => Object.assign(file, {
      preview: URL.createObjectURL(file as Blob)
    })))
  }, [setFiles]) //dependency callback
  const {getRootProps, getInputProps, isDragActive} = useDropzone({onDrop})

  return (
    <div {...getRootProps()} style={isDragActive ? {...dzStyles, ...dzActive} : dzStyles}>
      <input {...getInputProps()} />
      <Icon name='upload' size='huge' />
      <Header content='Drop image here' />
    </div>
  )
})