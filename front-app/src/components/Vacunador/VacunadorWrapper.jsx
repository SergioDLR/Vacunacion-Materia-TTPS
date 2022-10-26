import VacunadorNavBar from "../Nav/VacunadorNavBar";
const VacunadorWraper = ({ children }) => {
  return (
    <>
      <VacunadorNavBar />
      {children}
    </>
  );
};

export default VacunadorWraper;
