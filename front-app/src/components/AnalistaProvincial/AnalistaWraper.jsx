import AnalistaNavBar from "../Nav/AnalistaNavBar";
const AnalistaWraper = ({ children }) => {
  return (
    <>
      <AnalistaNavBar />
      {children}
    </>
  );
};

export default AnalistaWraper;
