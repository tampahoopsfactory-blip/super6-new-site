"""
EventShield Pro - Test CLI Commands
"""

import click
from flask.cli import AppGroup

test_group = AppGroup('test', help='Test runner commands')


@test_group.command('run')
def run_tests():
    """Run the test suite"""
    click.echo('Tests not configured yet')
