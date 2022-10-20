import OperadorNavBar from "../Nav/OperadorNavBar";
const OperadorWraper = ({ children }) => {
  return (
    <>
      <OperadorNavBar />
      {children}
    </>
  );
};

export default OperadorWraper;
