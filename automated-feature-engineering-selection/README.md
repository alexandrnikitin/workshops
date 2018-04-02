Quick start:

```bash
git clone -d 1 git@github.com:alexandrnikitin/workshops.git
cd workshops/automated-feature-engineering-selection/
docker run -d -p 8889:8888 -v ~/workshops/automated-feature-engineering-selection/:/home/jovyan/work/automated-feature-engineering-selection -e NB_UID=1000 -e GRANT_SUDO=yes --user root jupyter/datascience-notebook start-notebook.sh --NotebookApp.token=''
# more info https://github.com/jupyter/docker-stacks/tree/master/datascience-notebook
sudo pip install -r work/automated-feature-engineering-selection/requirements.txt
# dns issue https://stackoverflow.com/questions/28668180/cant-install-pip-packages-inside-a-docker-container-with-ubuntu

```