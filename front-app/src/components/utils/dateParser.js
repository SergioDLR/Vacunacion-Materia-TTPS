const dateParser = (date) => {
  const dateP = new Date(date);
  return `${dateP.getDate()}/${dateP.getMonth() + 1}/${dateP.getFullYear()}`;
};

export default dateParser;
